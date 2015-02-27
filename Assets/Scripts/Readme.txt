KiiWorld-Unity
================

A simple game to help you get started with Kii SDK on Unity 3D

Files
-----

There are 3 files in the Scripts folder that include Kii Cloud API calls:

1) GameTitle: used to sign-in/sign-up a user and initialize the backend
2) KiiCloudLogin: used to send asynchronous user sign-in/sign-up requests
   to the backend
3) ScoreManager: used to send and receive game high scores from the backend
   for each user
   
Build Config
------------

Build settings order:

Scenes/1_GameTitle.unity
Scenes/2_KiiCloudLogin.unity
Scenes/3_GameMain.unity
Scenes/4_GameClear.unity
Scenes/5_GameOver.unity

Tested platforms are PC/Mac, Android and iOS. We don't recommend running
this project under Web Player (because you'll run into networking 
limitations).

Create your own app backend
---------------------------

If you didn't get this project from developer.kii.com with preloaded
app id and key these are the instructions to make it work:

1) Create an account at http://developer.kii.com
2) Create an application as explained in "Register an application"
following steps 1, 2, and 3 (disregard the other sections):
http://documentation.kii.com/en/starts/unity/
Choose Unity as platform for your app and the server location of
your back-end.
3) Copy the App Id and App Key assigned to your app as explained in
"Register an application" following step 4 (disregard the other 
sections):
http://documentation.kii.com/en/starts/unity/
4) Replace those keys in file Scripts/GameTitle.cs in Kii.Initialize
method.
5) Run the GameTitle scene

Playing instructions
--------------------

1) Login or register a user
2) Fire the ball with the space key
3) Move the platform with the mouse
4) To sign off a user just sign in with a different user

Interested in Game Analytics?
-----------------------------

We also offer a dedicated Unity SDK for Game Analytics which you can
download here: http://developer.kii.com/#/sdks
More info: http://documentation.kii.com/en/guides/unity/managing-analytics