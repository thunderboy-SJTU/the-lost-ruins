using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

/// <summary>
/// A basic audio player class capable of playing songs from a playlist.
/// Provides basic navigation functions such as play, stop, next and previous.
/// Music may be played in a random order. The player is in continuous play mode
/// by default and stops only if being interrupted by setMusicEnabled(false)./// 
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {

    // the application manager object is optional and only needed for button focus
    public AppManager appManager;
    public Text intro;

    static public User user = new User();
    
    public Song[] songlist;
    private double[][] songs;
    private string[] singers;

    private AudioSource source;
    private AudioClip currentClip;

    public bool musicEnabled = false;
    public bool randomPlay = true;

    // index of the song currently being played
    public int currentSongIndex = 0;

    // VU meter displays
    public Image vuMeterL;
    public Image vuMeterR;

    // containing the actual volume magnitudes for each channel
    private float volumeL = 0;
    private float volumeR = 0;

    // helper for storing the last volume value
    private float lastVolumeL = 0;
    private float lastVolumeR = 0;

    // The song we are currently listening to
    public Text currentlyPlaying;
    public Text currentTime;

    // for the time display
    private string minutes;
    private string seconds;

    // stores the current elapsed time
    private static float timeElapsed;
    private bool firstin = true;
    void Start () {
        if (firstin == true) { 
            readsongs();
            firstin = false; 
        }

        
        if (musicEnabled)
        {
            source = GetComponents<AudioSource>()[0];
            intro.text = "歌随心动";

            if (randomPlay) currentSongIndex = UnityEngine.Random.Range(0, songlist.Length);


            currentSongIndex = calculate();
            print(currentSongIndex);

            displayintro(currentSongIndex);

            playSongAt(currentSongIndex);

        }
    }

    int calculate()
    {
        //StreamReader sr = new StreamReader("emotion.txt", Encoding.Default);
        String line = user.createData();
        if (line == "0 0 0 0 0 0 0 0 0 0 0"&&user.matchSinger=="") return UnityEngine.Random.Range(0, songlist.Length);
        double[] result = new double[songlist.Length];
        if (user.matchSinger != "")
        {
            for (int j = 0; j < songlist.Length; j++)
            {
                if (singers[j] == user.matchSinger)
                    return j;
            }
            return UnityEngine.Random.Range(0, songlist.Length);
        }

        //while ((line = sr.ReadLine()) != null)
        {
            String[] parts = line.Split(' ');
            for (int j = 0; j < songlist.Length; j++)
            {
                result[j] = 0;
                // for age
                result[j] += 1 / (Math.Abs(songs[j][0] - double.Parse(parts[0])) * 100);
                // for emotion
                for (int i = 1; i < parts.Length-1; i++)
                {
                    //Debug.Log(parts[i]);
                    if (parts[i].Length > 0) result[j] += double.Parse(parts[i]) * songs[j][i];
                }
            }
        }
        int tmp = 0;
        double max = result[0];
        //print(result[0]);
        for (int i = 1; i < result.Length; i++)
        {
            //print(result[i]);
            if (result[i] > max)
            {
                max = result[i];
                tmp = i;
            }
        }
        return tmp;
    }



    void readsongs()
    {
        StreamReader sr1 = new StreamReader("songs.txt", Encoding.Default);
        String line1;
        songs = new double[songlist.Length][];
        singers = new String[songlist.Length];
        int cnt = 0;
        while ((line1 = sr1.ReadLine()) != null)
        {
            String[] parts = line1.Split(' ');
            songs[cnt] = new double[parts.Length];
            for (int i = 0; i < parts.Length -1; i++)
            {
                songs[cnt][i] = double.Parse(parts[i]);
            }
            singers[cnt] = parts[parts.Length-1];
            cnt++;
        }
    }

    void displayintro(int index)
    {
        StringBuilder s = new StringBuilder();
        index++;
        switch (index)
        {
            case 1:
                s = new StringBuilder();
                s.Append("我爱你，").AppendLine();
                s.Append("为了寻找你，").AppendLine();
                s.Append("我搬进了鸟的眼睛，").AppendLine();
                s.Append("盯着路过的风，").AppendLine();
                s.Append("却也忘了听，猎人的枪声。").AppendLine();
                intro.text = s.ToString();
                break;
            case 2:
                s = new StringBuilder();
                s.Append("如果我们晚一点遇见").AppendLine();
                s.Append("我恰好温柔").AppendLine();
                s.Append("你恰好成熟").AppendLine();
                s.Append("我们会不会走到最后").AppendLine();
                intro.text = s.ToString();
                break;
            case 3:
                s = new StringBuilder();
                s.Append("暑假结束了，").AppendLine();
                s.Append("跟屁毛毛虫回家了，").AppendLine();
                s.Append("才发现有些不舍，").AppendLine();
                s.Append("有些想念。").AppendLine();
                s.Append("原来她是只可爱的小蝴蝶，").AppendLine();
                s.Append("悄悄飞入我们的夏天").AppendLine();
                intro.text = s.ToString();
                break;
            case 4:
                s = new StringBuilder();
                s.Append("每一支红色玫瑰的背后，").AppendLine();
                s.Append("都有它凄美的故事。").AppendLine();
                s.Append("所有的痛与梦，").AppendLine();
                s.Append("都在那个最漆黑最明朗的深夜绽放。").AppendLine();
                intro.text = s.ToString();
                break;
            case 5:
                s = new StringBuilder();
                s.Append("真正想要离开的那个人，").AppendLine();
                s.Append("挑一个风和日丽的下午，").AppendLine();
                s.Append("穿上一件大衣出门，").AppendLine();
                s.Append("消失在秋日的阳光里，").AppendLine();
                s.Append("再也没有回来").AppendLine();
                intro.text = s.ToString();
                break;
            case 6:
                s = new StringBuilder();
                s.Append("鲜花都到哪儿去了？ 鲜花都被姑娘们采去了").AppendLine();
                s.Append("姑娘们都到哪儿去了？ 姑娘们都被小伙子们娶走了").AppendLine();
                s.Append("小伙子们都到哪儿去了？ 小伙子们都去当兵打仗了").AppendLine();
                s.Append("士兵都到哪儿去了？ 士兵们都进坟墓了").AppendLine();
                s.Append("坟墓都到哪儿去了？ 坟墓都被鲜花覆盖了").AppendLine();
                intro.text = s.ToString();
                break;
            case 7:
                s = new StringBuilder();
                s.Append("我还是遇见了你，").AppendLine();
                s.Append("像柳絮遇到了桃花，青山遇到了云霞，").AppendLine();
                s.Append("你是独特风景的拟人和比喻，").AppendLine();
                s.Append("至少在此之前，").AppendLine();
                s.Append("美好只是空洞的形容词。").AppendLine();
                intro.text = s.ToString();
                break;
            case 8:
                s = new StringBuilder();
                s.Append("爱恋不过是一场暴雨").AppendLine();
                s.Append("你忘不了过去").AppendLine();
                s.Append("我忘不了你").AppendLine();
                s.Append("下雨了").AppendLine();
                s.Append("开始想你").AppendLine();
                intro.text = s.ToString();
                break;
            case 9:
                s = new StringBuilder();
                s.Append("江同学你好").AppendLine();
                s.Append("我是F班的袁湘琴").AppendLine();
                s.Append("我想你并不认识我 但是我对你却很了解喔").AppendLine();
                s.Append("第一次在新生训练上看到你").AppendLine();
                s.Append("那一天 我的眼光就不知道该怎么离开你").AppendLine();
                s.Append("仿佛你在哪里").AppendLine();
                s.Append("光就在哪里").AppendLine();
                intro.text = s.ToString();
                break;
            default:
                intro.text = "歌随心动";
                break;
        }
    }

	void Update () {

        // play next song if music is enabled, but no song is playing anymore
        if (source != null && !source.isPlaying && musicEnabled)
        {
            nextSong();
        }

        updateVU();
        updateTimer();
    }

    /// <summary>
    /// Let the VU meters reflect the current volume magnitude
    /// </summary>
    private void updateVU()
    {
        if (vuMeterL != null)
        {
            vuMeterL.fillAmount = Mathf.Lerp(lastVolumeL, volumeL, Time.deltaTime * 10);
        }
        if (vuMeterR != null)
        {
            vuMeterR.fillAmount = Mathf.Lerp(lastVolumeR, volumeR, Time.deltaTime * 10);
        }

        lastVolumeL = volumeL;
        lastVolumeR = volumeR;
    }

    /// <summary>
    /// Update the timer display of the player
    /// </summary>
    private void updateTimer()
    {
        minutes = Mathf.Floor(timeElapsed / 60).ToString("00");
        seconds = (timeElapsed % 60).ToString("00");

        if (currentTime != null)
        {
            currentTime.text = minutes + ":" + seconds;
        }

        if (source.isPlaying)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = 0;
        }
    }

    /// <summary>
    /// Update the label displaying the current song title
    /// </summary>
    private void updateSongInfo()
    {
        if (currentlyPlaying != null)
        {
            currentlyPlaying.text = songlist[currentSongIndex].name;
        }
        displayintro(currentSongIndex);
    }

    /// <summary>
    /// Load the next song, if we are currently playing the last song, the
    /// first song of the playlist is being played. If random play is enabled,
    /// simply the next random song is played.
    /// </summary>
    public void nextSong()
    {

        if (randomPlay)
        {
            currentSongIndex = UnityEngine.Random.Range(0, songlist.Length);
        }
        else
        {
            if (currentSongIndex < songlist.Length - 1)
            {
                currentSongIndex++;
            }
            else
            {
                currentSongIndex = 0;
            }

        }

        playSongAt(currentSongIndex);
        displayintro(currentSongIndex);
    }

    /// <summary>
    /// Plays a song at a spoecific index of the playlist
    /// </summary>
    /// <param name="index"></param>

    public void playSongAt(int index)
    {

        currentSongIndex = index;
        currentClip = songlist[currentSongIndex].clip;
        if (currentClip == null)
        {
            Debug.Log("Loading clip : " + "Music/" + songlist[currentSongIndex].name + ".mp3");
            currentClip = Resources.Load("Music/" + songlist[currentSongIndex].name, typeof(AudioClip)) as AudioClip;
            songlist[currentSongIndex].clip = currentClip;

            // Resources.LoadAssetAtPath("Assets/Music/"+songlist[currentSongIndex].name, typeof(AudioClip));
        }
        source.clip = currentClip;
        timeElapsed = 0;
        updateSongInfo();
        source.Play();
        displayintro(currentSongIndex);

        if (appManager != null)
        {
            appManager.setButtonSelected(currentSongIndex);
        }

    }

    /// <summary>
    /// Load the previous song, if we are currently playing the first song, the
    /// last song of the playlist is being played. If random play is enabled,
    /// simply the next random song is played.
    /// </summary>
    public void previousSong()
    {

        if (randomPlay)
        {
            currentSongIndex = UnityEngine.Random.Range(0, songlist.Length);
        }
        else
        {
            if (currentSongIndex > 0)
            {
                currentSongIndex--;
            }
            else
            {
                currentSongIndex = songlist.Length - 1;
            }

        }

        playSongAt(currentSongIndex);
        displayintro(currentSongIndex);
    }

    /// <summary>
    /// Stop playing and reset the VU meters
    /// </summary>
    public void stop()
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
            intro.text = "歌随心动";

            if (currentlyPlaying != null)
            {
                currentlyPlaying.text = "";
            }

        }

        volumeL = 0;
        volumeR = 0;

    }

    /// <summary>
    /// Sets the music enabled or disabled, the music starts immediately, if enabled
    /// </summary>
    /// <param name="enabled"></param>
    public void setMusicEnabled(bool enabled)
    {
        this.musicEnabled = enabled;

        if (enabled)
        {
            source = GetComponents<AudioSource>()[0];
            int index = UnityEngine.Random.Range(0, songlist.Length);
            playSongAt(index);
        }
        else
        {
            source = GetComponents<AudioSource>()[0];

            if (source.isPlaying)
            {
                source.Stop();
                intro.text = "歌随心动";

                if (currentlyPlaying != null)
                {
                    currentlyPlaying.text = "";
                }

                volumeL = 0;
                volumeR = 0;
            }
        }

    }

    /// <summary>
    /// Set the volume of the associated AudioSource
    /// </summary>
    /// <param name="volume"></param>
    public void setVolume(float volume)
    {
        source = GetComponents<AudioSource>()[0];
        source.volume = volume;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        float valL = 0;
        float valR = 0;

        // we have interleaved signals here, so we have to split them by channel
        for (var i = 0; i < data.Length - 1; i += 2)
        {
            // rectify data and add current sample for each channeö
            valL += Mathf.Abs(data[i]);
            valR += Mathf.Abs(data[i + 1]);
        }

        // average samples to get the magnitude
        // should be data.length / 2 but that gives the wrong amplitude
        valL /= data.Length / 4;
        valR /= data.Length / 4;

        // finally assign the value only if it has changed
        if (valL != volumeL)
        {
            volumeL = valL;
        }
        if (valR != volumeR) {         
            volumeR = valR;
        }

    }

}
