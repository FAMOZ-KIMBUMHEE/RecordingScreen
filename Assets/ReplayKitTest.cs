using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using NatSuite.Sharing;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;


public class ReplayKitTest : MonoBehaviour
{
    //Custom
    [SerializeField] bool recordMicrophone; //����ũ ��뿩��(����ũ�� ����������� ��� �������� ���) 
    [SerializeField] Camera cam; //��ȭ�� ī�޶�
    [SerializeField] AudioClip audioClip; //���� �� ��������Ŭ�� <- ��ȭ�ϰ��� �ϴ� ������ �־��ָ��
    [SerializeField] AudioSource listenerAudioSource;//��ȭ�� ������ ������ �Ҹ��� �ȳ����⋚����, ������ �ΰ� Ʋ�����. <- �����׿� ����
    [SerializeField] Button startBtn, stopBtn, shareBtn,previewBtn;

    //asset
    private CameraInput cameraInput;
    private MP4Recorder recorder;
    private IClock clock;
    private AudioSource recordAudioSource;
    private AudioInput audioInput;
    string recordPath = "";

    void PermissionSet()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    //�� �ʱ�ȭ
    private void Start()
    {
        PermissionSet();

        // Start microphone
        recordAudioSource = gameObject.AddComponent<AudioSource>();
        recordAudioSource.mute =
        recordAudioSource.loop = true;
        recordAudioSource.bypassEffects =
        recordAudioSource.bypassListenerEffects = false;
        recordAudioSource.clip =  recordMicrophone ? Microphone.Start(null, true, 1, AudioSettings.outputSampleRate) : audioClip;
        listenerAudioSource.clip = recordAudioSource.clip;
        stopBtn.interactable = false;
        startBtn.interactable = true;
        shareBtn.interactable = false;
        previewBtn.interactable = false;
    }

    //��ȭ����
    public void StartRecording()
    {
        // Start recording
        var frameRate = 30;
        var sampleRate = AudioSettings.outputSampleRate;
        var channelCount = (int)AudioSettings.speakerMode;
        var clock = new RealtimeClock();
        recorder = new MP4Recorder(Screen.width, Screen.height, frameRate, sampleRate, channelCount, audioBitRate: 96_000);
        // Create recording inputs
        cameraInput = new CameraInput(recorder, clock, cam);
        audioInput = new AudioInput(recorder, clock, recordAudioSource, true);
        // Unmute microphone
        recordAudioSource.mute = audioInput == null;
        recordAudioSource.Play();
        listenerAudioSource.Play();
        startBtn.interactable = false;
        stopBtn.interactable = true;
    }

    //��ȭ����
    public void StopRecording()
    {
        recordAudioSource.Stop();
        listenerAudioSource.Stop();
        // Mute microphone
        recordAudioSource.mute = true;
        // Stop recording
        audioInput?.Dispose();
        cameraInput.Dispose();
        
        // Playback recording
        SaveRecord();

    }

    //��������
    public async void SaveRecord()
    {
        stopBtn.interactable = false;

        recordPath = await recorder.FinishWriting();

        startBtn.interactable = true;
        shareBtn.interactable = true;
        previewBtn.interactable = true;

    }

    //�̸�����
    public void Preview()
    {
        if(recordPath != "")
        {
            Handheld.PlayFullScreenMovie($"file://{recordPath}");
        }
    }

    public void SharedRecord()
    {
        new NativeShare().AddFile(recordPath).SetTitle("TestVideo").SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        .Share(); ;
    }

    private void OnDestroy()
    {
        // Stop microphone
        recordAudioSource.Stop();
        listenerAudioSource.Stop();
        Microphone.End(null);
    }

}
