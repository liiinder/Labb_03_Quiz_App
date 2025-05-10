# Quiz App - How to setup

- Clone this repo into you prefered IDE
- Open the solution file (Labb_03_Quiz_App.sln)
- And run the program
- A JSON-file will be created on startup in %AppData%\Local\Labb_03_Quiz_App which will store all the QuizPacks.

<br />

# About the process
This app was our [third assignment](/ASSIGNMENT.md) in our first course programming with C#.
We got introduced to a lot of things including WPF, JSON, API's and the MVVM-pattern.
I have done some work with JSON and API's before so my main focus for this assignment was to try and learn WPF/XAML as good as possible!

When our teacher presented the assignment he showed us a working app. I took his design and made my app look like his, but I refined it in many ways including rounded corners on all elements, color gradient on buttons to make them pop more and actually look like buttons, better aligned text/buttons/inputs etc.

The biggest problems I encountered during this project was funnily enough to make the corner-radius on the comboboxes and overall how to style elements in WPF. It wasn't as straight forward as CSS that I'm more used to and simple things took some time to figure out. The MVVM-pattern was also pretty rought as I didn't feel like it had enough of "This is the best practice" and it was more of a "you can do it like this or this" so was pretty hard to figure out exactly how to open dialogs etc the "correct" way.

Also one of the biggest AHA-moment I got during this project was when my classmates where discussing about how to show the right answer, as WPF's default behaviour when hovering a button was changing the color to blue and that was covering the color change and using images felt much harder. That AHA-moment I got was that I could just always have an image displayed on the buttons and just change the url like I changed the colors as an image without an url wont be displayed. In just 30 minutes the answers where also shown with an image and it made the app look much better and the code felt pretty efficient!

In the end I'm very pleased and proud over the result.

<br/>

# Demo pictures

When you start the app your QuizPack gets loaded from a locally stored JSON-file and it saves when you close.

In the configuration mode you have the title of the quiz on the top left, followed by buttons for editing the QuizPack and buttons for add and remove questions.
Then the questions is displayed on the left and if you select a question you can edit it and its answers on the right.

![](/presentation_images/CustomizeQuizPacks.jpg)

If you click the button for configure the quiz you get a dialog window where you can change the name/difficulty/time-limit for the specific pack.

![](/presentation_images/EditQuizPack.jpg)

In the File menu option you could create a new pack or swap between different packs you've already made. The menu is setup to have support for Alt navigation so ` Alt -> F -> N ` creates a new QuizPack and it also have support for hotkeys like ` Insert ` to add a question ` Delete ` to remove, ` Ctrl + E or P ` to swap between Edit and Play-mode.

![](/presentation_images/ChangeQuizPack.JPG)

There is also an option to import questions from [Open Trivia Database](https://opentdb.com/).
The user then gets to choose a category and difficulty if they want and then how many questions they want to import.
It then uses an JWT-Token to remember us so if we then asks for more questions it wont give us the same questions.
And if there is any errors or no more questions to get with that specific category/difficulty combination the user will get a error message explaining what happened.

![](/presentation_images/OpenTdb.jpg)

When you're done with setting up your quiz you can play it! All the questions get randomized and then presented to the player and the player needs to answer within the time the QuizPack is set to. The answers position is also randomized so the players can't just remember it's position. When the player makes a guess the game shows us if it was right or wrong and show us the right answer. And when all questions are answered you get to see how many you got right and a button to restart.

![](/presentation_images/PlayQuiz.JPG)
![](/presentation_images/Result.jpg)


<br/>

Thanks for reading!<br/>
/ Kristoffer Linder