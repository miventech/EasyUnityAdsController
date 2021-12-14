using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsController : MonoBehaviour ,IUnityAdsInitializationListener,IUnityAdsLoadListener,IUnityAdsShowListener
{
    public const string GameID = "4503987";
    public const string InterstitialID  = "Android_Interstitial";
    public const string rewardVideoID  = "Android_Rewarded";
    public const string bannerID  = "Android_Banner";
  
    public const bool testMode = false;
    public static AdsController main;
    public int numeroDeVecesEnPantalla = 0;
    public int counter = 1;

    public bool insterticialReady  = false;
    public bool insterticialCalled  = false;
    public bool videoReady  = false;
    public bool VideoCalled  = false;
    public bool initializedAds  = false;

    // Start is called before the first frame update
    void Start()
    {   
        if(main == null){
            main = this;
        }else{
            checkInstance();
            Destroy(this.gameObject);
            return;
        }
        checkInstance();
        Advertisement.Initialize(GameID, testMode , this);
        StartCoroutine(updateAds());
        DontDestroyOnLoad(this.gameObject);
       
    }
    public IEnumerator updateAds(){
        yield return new WaitForSeconds(1);
        while (!initializedAds)
        {
            yield return new WaitForSeconds(5);
            Advertisement.Initialize(GameID, testMode , this);
        }
        while (true)
        {
             preparateReward();
             preparateInsterticial();
             yield return new WaitForSeconds(3f);
        }
    }
    public void preparateReward(){
        if(!videoReady && !VideoCalled){
            Advertisement.Load(rewardVideoID,this);
            VideoCalled = true;
        }
    }
    public void preparateInsterticial(){
        if(!insterticialReady && !insterticialCalled){
              insterticialCalled = true; 
              Advertisement.Load(InterstitialID,this);
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)){
            Debug.Log("Print ScreenShot");
            ScreenCapture.CaptureScreenshot( Application.dataPath + "/" + System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".png");
        }
    }
    public void checkInstance(){
        if(counter == 0) return;
        main.numeroDeVecesEnPantalla += counter;
        if(main.numeroDeVecesEnPantalla >= 2 ){
            main.showInterticial();
        }
    }
    

    public void showVideo(){
        if(videoReady){
            Advertisement.Show(rewardVideoID ,this);
        }
    }


    public void showInterticial(){
        Debug.Log("Show Intericial" + main.insterticialReady.ToString());
        if(main.insterticialReady == true){
            Debug.Log("Show IntericialIF");
            main.numeroDeVecesEnPantalla = 0;
            Advertisement.Show(InterstitialID , this);
        }
    }

    ///
    ///
    ///
    ///
    ///


    ///
    ///
    ///
    ///
    ///
    /// 
    /// //initilized call
    public void OnInitializationComplete()
    {
        initializedAds = true;
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }


    //load calls

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if(placementId == rewardVideoID){
            videoReady = true;
        }
        if( placementId == InterstitialID){
            insterticialReady = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        showedAnyAds(placementId);
    }


    //Show called
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        showedAnyAds(placementId);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        showedAnyAds(placementId);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        showedAnyAds(placementId);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        showedAnyAds(placementId);
        if(placementId == rewardVideoID){
            activatePowerUP.recompenzaVideo();
        }
    }

    void showedAnyAds(string placementId){
        if(placementId == rewardVideoID){
            videoReady = false;
            VideoCalled = false;
        }
        if( placementId == InterstitialID){
            insterticialReady = false;
            insterticialCalled = false;
        }
    }
}
