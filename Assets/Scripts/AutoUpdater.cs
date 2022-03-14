using System.IO;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using System.ComponentModel;
using System.Xml;
//using System.IO.Compression;

public class AutoUpdater : MonoBehaviour
{
    [SerializeField]
    string url;
    public static string version = "v.0.0.1";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(File.Exists(Application.persistentDataPath + "/Updater.app"));
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
            Debug.Log("Github response successed");
            Debug.Log(str);
            GithubResponse response = JsonConvert.DeserializeObject<GithubResponse>(str);
            Debug.Log(response.TagName);
            if (response.TagName != version)
            {
                //DownloadFile(response.Assets[1].BrowserDownloadUrl, "Updater.zip",
                //    Application.persistentDataPath, true,
                //    OnDownloadingUpdaterFinished);

                DownloadFile(response.Assets[0].BrowserDownloadUrl, "NewVersion.zip",
                    Application.dataPath + "/../../", false, "NewVersion.app",
                    OnDownloadingNewVersionFinished);
            }
        }
    }

    void DownloadFile(string Url, string name, string savePath, bool isZip, string nameAfterUnzip, Action onComplete)
    {
        WebClient client = new WebClient();
        void OnDownloadComplete(object obj, AsyncCompletedEventArgs a)
        {
            if (File.Exists(Application.dataPath + "/../" + name))
            {
                File.Move(Application.dataPath + "/../" + name, savePath + name);
                if (isZip)
                {
                    UnzipFile(savePath, name, nameAfterUnzip);
                }
            }
            onComplete();
        }
        client.DownloadFileCompleted += OnDownloadComplete;
        client.DownloadFileTaskAsync(Url, name);
    }

    //void OnDownloadingUpdaterFinished(){}

    void OnDownloadingNewVersionFinished()
    {
        Save(Application.dataPath + "/../NewVersion.zip", Application.dataPath + "/../AoTTG.app",
            Application.persistentDataPath + "/UpdaterConfig.wow");
        if (File.Exists(Application.persistentDataPath + "/Updater.app"))
            System.Diagnostics.Process.Start(Application.persistentDataPath + "/Updater.app");
    }

    // TODO find some way to unzip the file downloaded
    void UnzipFile(string path, string nameBefore, string nameAfter)
    {
        //GZipStream stream = new GZipStream(File.Open(path + nameBefore, FileMode.Open), CompressionMode.Decompress);
        //FileStream stream1 = File.Create(path + nameAfter);
        //stream.CopyTo(stream1);
    }

    void Save(string newVersionPath, string currentVersionPath, string savePath)
    {
        XmlDocument xml = new XmlDocument();
        XmlElement root = xml.CreateElement("Root");
        XmlElement element1 = xml.CreateElement("newVersionPath");
        XmlElement element2 = xml.CreateElement("currentVersionPath");
        element1.InnerText = newVersionPath;
        element2.InnerText = currentVersionPath;
        root.AppendChild(element1);
        root.AppendChild(element2);
        xml.AppendChild(root);
        xml.Save(savePath);
        Debug.Log(savePath);
    }
}

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