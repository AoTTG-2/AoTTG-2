using System.IO;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using System.ComponentModel;
using TMPro;
using System.Security.Cryptography;

enum EPlatform
{
    Windows = 0,
    Mac = 1,
    Linux = 2
}

public class AutoUpdater : MonoBehaviour
{
    [SerializeField]
    GameObject updatePanel, updateCheckPanel, warningPanel;

    [SerializeField]
    TMP_Text versionText, downloadingText;

    [SerializeField]
    string version = "v1.8", hashCode = "";

    public bool disableUpdater = false;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
    bool usePreRelease = true;
    string url = "https://api.github.com/repos/AoTTG-2/AoTTG-2/releases/61795699";
#else
    bool usePreRelease = false;
    string url = "https://api.github.com/repos/AoTTG-2/AoTTG-2/releases/latest";
#endif

    GithubResponse response;

    EPlatform platform;

    void Start()
    {
        if (disableUpdater)
        {
            Destroy(updatePanel);
            return;
        }
        updatePanel.SetActive(false);
        if (!usePreRelease)
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
                platform = EPlatform.Windows;
            else if (Application.platform == RuntimePlatform.OSXPlayer)
                platform = EPlatform.Mac;
            else if (Application.platform == RuntimePlatform.LinuxPlayer)
                platform = EPlatform.Linux;
        }
        StartCoroutine(GetGithubResponse(url));
    }

    IEnumerator GetGithubResponse(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
        }

        else if (request.result == UnityWebRequest.Result.Success)
        {
            var str = request.downloadHandler.text;

            response = JsonConvert.DeserializeObject<GithubResponse>(str);
            if (response.TagName != version)
            {
                updatePanel.SetActive(true);
                updateCheckPanel.SetActive(true);
                downloadingText.gameObject.SetActive(false);
                warningPanel.SetActive(false);
                versionText.text = response.TagName;
            }
            else
            {
                Destroy(updatePanel);
            }
        }
    }

    void OnDownloadingNewVersionFinished()
    {
        downloadingText.text = "Checking...";
        using (var md = MD5.Create())
        {
            using (var file = File.OpenRead(Application.dataPath + "/../../NewVersion.zip"))
            {
                var hash = BitConverter.ToString(md.ComputeHash(file));
                if (hash == hashCode)
                {
                    downloadingText.text = "Complete!";
                    System.Diagnostics.Process.Start(Application.dataPath + "/../../NewVersion.zip");
                    Application.Quit();
                }
                else
                {
                    //Debug.Log(hash);
                }
            }
        }
    }

    void OnDownloadProgressCanged(object obj, DownloadProgressChangedEventArgs args)
    {
        downloadingText.text = "downloiading...        " + args.BytesReceived + "/" + response.Assets[(int)platform].Size + "  (" + args.ProgressPercentage + "%)";
    }

    void DownloadFile(string Url, string name, string savePath, Action onComplete, DownloadProgressChangedEventHandler progressChangeEvent)
    {
        WebClient client = new WebClient();
        void OnDownloadComplete(object obj, AsyncCompletedEventArgs a)
        {
            if (File.Exists(Application.dataPath + "/../" + name))
            {
                File.Move(Application.dataPath + "/../" + name, savePath + name);
            }
            onComplete();
        }
        client.DownloadFileCompleted += OnDownloadComplete;
        client.DownloadFileTaskAsync(Url, name);
        client.DownloadProgressChanged += progressChangeEvent;
    }

    public void OnUpdateButtonClicked()
    {
        DownloadFile(response.Assets[(int)platform].BrowserDownloadUrl,
            "NewVersion.zip", Application.dataPath + "/../../",
            OnDownloadingNewVersionFinished, OnDownloadProgressCanged);

        updateCheckPanel.SetActive(false);
        downloadingText.gameObject.SetActive(true);

        downloadingText.text = "downloiading...";
    }

    public void OnSkipButtonClicked()
    {
        warningPanel.SetActive(true);
    }

    public void OnLemmeThinkButtonClicked()
    {
        warningPanel.SetActive(false);
    }

    public void OnYesButtonClicked()
    {
        Destroy(updatePanel);
    }

}

#region Github Response classes

public class Author
{
    [JsonProperty("login")]
    public string Login { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonProperty("gravatar_id")]
    public string GravatarId { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; }

    [JsonProperty("followers_url")]
    public string FollowersUrl { get; set; }

    [JsonProperty("following_url")]
    public string FollowingUrl { get; set; }

    [JsonProperty("gists_url")]
    public string GistsUrl { get; set; }

    [JsonProperty("starred_url")]
    public string StarredUrl { get; set; }

    [JsonProperty("subscriptions_url")]
    public string SubscriptionsUrl { get; set; }

    [JsonProperty("organizations_url")]
    public string OrganizationsUrl { get; set; }

    [JsonProperty("repos_url")]
    public string ReposUrl { get; set; }

    [JsonProperty("events_url")]
    public string EventsUrl { get; set; }

    [JsonProperty("received_events_url")]
    public string ReceivedEventsUrl { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("site_admin")]
    public bool SiteAdmin { get; set; }
}

public class Uploader
{
    [JsonProperty("login")]
    public string Login { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonProperty("gravatar_id")]
    public string GravatarId { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; }

    [JsonProperty("followers_url")]
    public string FollowersUrl { get; set; }

    [JsonProperty("following_url")]
    public string FollowingUrl { get; set; }

    [JsonProperty("gists_url")]
    public string GistsUrl { get; set; }

    [JsonProperty("starred_url")]
    public string StarredUrl { get; set; }

    [JsonProperty("subscriptions_url")]
    public string SubscriptionsUrl { get; set; }

    [JsonProperty("organizations_url")]
    public string OrganizationsUrl { get; set; }

    [JsonProperty("repos_url")]
    public string ReposUrl { get; set; }

    [JsonProperty("events_url")]
    public string EventsUrl { get; set; }

    [JsonProperty("received_events_url")]
    public string ReceivedEventsUrl { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("site_admin")]
    public bool SiteAdmin { get; set; }
}

public class Asset
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("label")]
    public object Label { get; set; }

    [JsonProperty("uploader")]
    public Uploader Uploader { get; set; }

    [JsonProperty("content_type")]
    public string ContentType { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("size")]
    public int Size { get; set; }

    [JsonProperty("download_count")]
    public int DownloadCount { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("browser_download_url")]
    public string BrowserDownloadUrl { get; set; }
}

public class GithubResponse
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("assets_url")]
    public string AssetsUrl { get; set; }

    [JsonProperty("upload_url")]
    public string UploadUrl { get; set; }

    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("author")]
    public Author Author { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("tag_name")]
    public string TagName { get; set; }

    [JsonProperty("target_commitish")]
    public string TargetCommitish { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("draft")]
    public bool Draft { get; set; }

    [JsonProperty("prerelease")]
    public bool Prerelease { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }

    [JsonProperty("assets")]
    public List<Asset> Assets { get; set; }

    [JsonProperty("tarball_url")]
    public string TarballUrl { get; set; }

    [JsonProperty("zipball_url")]
    public string ZipballUrl { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }
}

#endregion