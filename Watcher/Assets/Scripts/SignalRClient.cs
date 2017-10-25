using SignalR.Client._20.Hubs;
using System;
using System.Collections.Generic;
using UnityEngine;
 
// ref: https://github.com/jenyayel/SignalR.Client.20 
public class SignalRClient : MonoBehaviour {
    private int _floatDirection = 1;
    private HubConnection _hubConnection = null;
    private IHubProxy _proxy = null;
    private string _hubUri = "http://localhost:8080";
    private Dictionary<ulong, Tracker> _tracker = null;
    private ulong _lastTracked = 0;
    public float _movementMultiplier = 1;
    public float _xMultiplier = 1;
    public float _yMultiplier = 1;
    public float _zMultiplier = 1;
    public GameObject _watcher = null;
    public Camera _mainCamera = null;
    public Camera _displayCamera = null;
    public Light _leftEye = null;
    public Light _rightEye = null;
    public int _trackingTime = 2;
    public float _trackerXRotation = 0;

    // Use this for initialization
    void Start () {


        try
        {
            Log("Starting");

            this._hubConnection = new HubConnection(this._hubUri);
            this._proxy = this._hubConnection.CreateProxy("BodyHub");

            this._proxy.Subscribe("headTracked").Data += SignalRClient_Data;

            this._hubConnection.Start();

            this._mainCamera.enabled = false;
            this._displayCamera.enabled = true;

            Log("Started");
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }
	}

    private void SignalRClient_Data(object[] obj)
    {
        try
        {

            HeadTrackingMsg msg = null;
            string json = String.Empty;
            
            json = obj[0].ToString();            
            msg = JsonUtility.FromJson<HeadTrackingMsg>(json);

            // store the tracking messages     
            if (this.HeadTracker.ContainsKey(msg.Person))
            {
                this.HeadTracker[msg.Person].person = msg;
                this.HeadTracker[msg.Person].time = DateTime.Now;
            }
            else
            {
                this.HeadTracker[msg.Person] = new Tracker() { person = msg, time = DateTime.Now, item = null, tracked = DateTime.Now };
            }
        }catch(Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    // Update is called once per frame
    void Update () {
        List<ulong> oldKeys = new List<ulong>();
        ulong trackingid = 0;
        //List<ulong> keys = new List<ulong>();
        DateTime firstTracked = DateTime.MaxValue;

        // remove people we havent seen within the tracking interva
        foreach(ulong key in this.HeadTracker.Keys)
        {
            if(DateTime.Now.Subtract(this.HeadTracker[key].time).TotalSeconds > this._trackingTime)
            {
                oldKeys.Add(key);
                GameManager.Destroy(this.HeadTracker[key].item);
                //this.HeadTracker.Remove(key);                
            }
        }

        foreach(ulong k in oldKeys)
        {
            Log(String.Format("Removing: {0})", k));
            this.HeadTracker.Remove(k);
        }

        // update the people
        foreach (Tracker tracker in this.HeadTracker.Values)
        {
            if(tracker.tracked < firstTracked)
            {
                firstTracked = tracker.tracked;
                trackingid = tracker.person.Person;
            }
            
            if (tracker.item == null)
            {
                tracker.item = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            }

            // set the location attributes
            tracker.item.transform.position = this._movementMultiplier *  (new Vector3(tracker.person.X * this._xMultiplier, tracker.person.Y * this._yMultiplier, tracker.person.Z * this._zMultiplier));           
        }
        
        if (trackingid > 0 && this._watcher != null)
        {
            Transform target = this.HeadTracker[trackingid].item.transform;
            Vector3 tVector = new Vector3(target.position.x * -1f, target.position.y, target.position.z);
            
            this._leftEye.gameObject.SetActive(true);
            this._rightEye.gameObject.SetActive(true);
            this._watcher.transform.LookAt(tVector);

            Log(String.Format("Tracking: [{0}]: {1}, {2}, {3}", new object[] { this.HeadTracker[trackingid].person, tVector.x, tVector.y, tVector.z }));
        }
        else
        {
            this._leftEye.gameObject.SetActive(false);
            this._rightEye.gameObject.SetActive(false);

            // bouncing head
            if (this._watcher.transform.position.y > .4f)
            {
                this._floatDirection = -1;
            }
            else if (this._watcher.transform.position.y < -.4f)
            {
                this._floatDirection = 1;
            }

            this._watcher.transform.position = new Vector3(this._watcher.transform.position.x, this._watcher.transform.position.y + (float)(this._floatDirection * .005), this._watcher.transform.position.z);
        }

        
        
            
       
        
    }

    public static void Log(string message)
    {
        Log(message, DateTime.Now);
    }

    public static void Log(string message, DateTime time)
    {
        string text = String.Format("{0}: {1}", time.ToString("yyyy.MM.dd HH:mm:ss.fff"), message);
        Debug.Log(text);
    }

    internal Dictionary<ulong, Tracker> HeadTracker
    {
        get
        {
            lock(this)
            {
                if(this._tracker == null)
                {
                    this._tracker = new Dictionary<ulong, Tracker>();
                }
            }

            return this._tracker;
        }
    }
}

[System.Serializable]
internal class HeadTrackingMsg
{
    public ulong Person;
    public float X;
    public float Y;
    public float Z;
}

internal class Tracker
{
    public DateTime tracked;
    public DateTime time;
    public GameObject item;
    public HeadTrackingMsg person;
}