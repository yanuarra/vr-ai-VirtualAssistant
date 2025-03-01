'SentisSpeechRecog' Project - Speech Recognition AI Running Locally On Meta Quest
=================================================================================

This Unity project demonstrates the use of Unity Sentis to run a Neural Network model locally on the Meta Quest 3. Specifically, it utilizes the 'Whisper Tiny' speech recognition model from Hugging Face.


Included Assets
---------------
This project includes the "Sentis Whisper Tiny" model, which was obtained from here:
https://huggingface.co/unity/sentis-whisper-tiny


License
-------
This project is licensed under the MIT License (see LICENSE.txt). However, it includes components that are licensed under different terms:

- The "Sentis Whisper Tiny" model is licensed under the Apache License 2.0. A copy of the license can be found here:
https://huggingface.co/datasets/choosealicense/licenses/blob/main/markdown/apache-2.0.md


Project Setup 
-------------
Open the Project in Unity Editor.

- Go to File > Build Settings.
- Select 'Android' from the list of platforms.
- Set 'Texture Compression' to 'ASTC'.
- Click 'Switch Platform' to make Android the active build target.

- Use this scene: Scenes/SpeechRecogScene
- With your Meta Quest headset connected, and 'Run Device" set to 'Oculus Quest 3', click the 'Build and Run' button. This will deploy the app to your headset.


How to Use the App
------------------

- Press and hold down the left controller's Trigger button to record yourself speaking.
- Release the left controller's Trigger button to stop recording.
- Press the right controller's Trigger button to transcribe the recorded speech to text.


Disclaimer 
----------
The Unity project is provided "as is" without warranty of any kind. Use at your own risk. The authors are not responsible for any damage or data loss resulting from the use of this package. Compatibility and performance may vary depending on your system configuration.