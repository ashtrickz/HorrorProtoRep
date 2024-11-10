<h2> Character Controller </h2>
<p> First of all project has Player Controller with various managers for Player to function in any ways. Player can walk, sprint, crouch, pick/switch/drop items, use them if intended, press buttons/switches, open doors. All the parameters and data for managing this functions is easy to configure without changing a single line of code.</p>

<video src=https://github.com/user-attachments/assets/96edad83-75c8-479d-8c86-60105edd3e17></video>

<p> Shortly about Managers and main components</p>
<ul>
  <li>Manager for handling all Player's Sub Managers:</li>
    <img src=https://github.com/user-attachments/assets/30ae3e48-310c-46e7-ba77-1997f6bc66b5>
  <li>Controller based on Character Controller component with custom gravity updating in <strong>Update()</strong>:</li>
    <img src=https://github.com/user-attachments/assets/ddf4f01f-607a-47a0-be05-14317fb209f0>
  <li>Input Manager with custom logic above <i>New Input System</i> Manager:</li>
	<img src=https://github.com/user-attachments/assets/2f7478a7-b656-4e2a-8d5a-08442dad6f8d>
  <li>Camera Manager with general camera settings:</li>
	<img src=https://github.com/user-attachments/assets/f4c0fe9e-1049-46f4-91b4-ef9b7bd6afc8>
  <li>Interaction Manager responsible for Player's interactions with Items, Doors, Buttons etc.</li>
	<img src=https://github.com/user-attachments/assets/7212a69d-6cc0-483f-a25e-eda6cb0862bf>
  <li>Inventory Manager which is holding all player's inventory data and working with it, making Picking Up, Switching, Droping, Using Items possible:</li>
	<img src=https://github.com/user-attachments/assets/dade3111-9aa7-41bb-8e0a-5aca50b24c23>
  <li>View Bobbing script for making First Person View experience more realistic and dynamic</li>
	<img src=https://github.com/user-attachments/assets/b65ee5a0-3959-4e5e-8269-5e666aa307e8>
</ul>
<h2> Interactions </h2>
<p> Thanks to player's Interaction Manager we can interact with different parts of environment. With this project you can easely create and configure all the interactions you could ever need while creating Horror games and so on. Also all interactable objects are outlined. If you want interactable to be interacted only with mechanism (button for example) it's enough to just change it's layer from Interactable to any other.</p>
<video src=https://github.com/user-attachments/assets/5d2cc9bf-bce7-48ef-935a-e5b7414c279f></video>
<video src=https://github.com/user-attachments/assets/559765e2-9447-421f-8348-35bd5add83ce></video>
<p>At the moment we have <strong>MechanismBehaviour</strong> script for creating any kind of Buttons and Switches which can be easely modified to create new types of interactions. So Door script can be modified the same way. With just changing type of Mechanism you can change button into switch and back with custom animation parameters.</p>
<video src=https://github.com/user-attachments/assets/064d87fe-b65d-467f-acc5-ffc31869ab77></video>
<h2> AI Monster </h2>
<p> In project you can find AI Monster Dummy working on Finite State Machine and Nav Mesh Agent. He has various states that can be configured right in it's prefab.</p>

![image](https://github.com/user-attachments/assets/3eb5ad5a-f215-4753-b85d-d1d1ff8d3432)

<p>Fully compatible with animator, animations and blend trees. For the test we have Idle, Patrol, Chase, Run, Attack and Hit states.</p>
<video src=https://github.com/user-attachments/assets/1a626248-42ab-40c2-b078-b881c7177d23></video>

<p>For visualisation and easier configuration all properties are drawn with Gizmo.</p>

![image](https://github.com/user-attachments/assets/be6926e7-ebab-412b-9486-111a08af64cf)

<p> At the moment there is only little misscalculation with FOV to be fixed ASAP.</p>

<h2> Data Handling </h2>
<p> All the main data is stored in RootData Scriptable Singleton. In the future it will contain all the items data, main player configurations and more. For now it's only main game events that can be reached from any part of the code for creating dynamic callbacks and more.

![image](https://github.com/user-attachments/assets/2b5af20e-52be-4a49-a146-d0a9fc1ad8d4)

<p>Every single item has it's Scriptable Data with main parameters. In the future it'll also contain collider data, rigidbody data and more to make In-Game Item Spawner (Pooler). Also i will add custom parameters to items to make them even more customizable without writing or changing code. </p>

![image](https://github.com/user-attachments/assets/b658d205-bc21-489e-9779-3cd60220fde4)










