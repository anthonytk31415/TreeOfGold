# TreeOfGold
TreeOfGold is written using the Unity platform in C# and is a 2D Turn-Based fantasy strategy game. Command your team of elite fighters and vanquish your foes to save the world from impending doom!

TreeOfGold began as a project to showcase, develop, and refine my ability to code a large project, utilizing design principles and architecture to make a scalable and multifaced application. It has grown to be a labor of love to not only dive deep into Unity and C# but also create what is hopefully an enjoyable and in-depth playing experience for players all over.

Software Engineering Highlights: 
- The game uses C# and Unity and is built using an MVC architectural pattern. 
- Uses a Message Queue to process and handle character animations, in-game AI decisions, user input.
- To handle State for various elements of the game (for example, types of movement during the player phase of the game, animations, User Interfaces, Game Modes, etc.) I leverage the State design pattern. 
- Utilized graph algorithms and queues (e.g. bread-first-search to look at mininum distance from character A to character B; queues for sequencing commands and outcomes on interactions between characters) with State design for AI interactions and rendering decisions.
- Designed UI tools to automate and streamline creation of characters, levels, and various UI elements. I implement this using Editor functions in the Editor menu for functionality testing, granting GameObjects functionality (like adding components) 
- Implemented Observer pattern using Delegates and Event Listeners to implement UI/Animations. 
- I create a base character that all other characters will inherit from. Scripts are implemented for animations (e.g. to swap relevant sprites for animations), grant properties and types of commands, for scalability and decoupling. 
