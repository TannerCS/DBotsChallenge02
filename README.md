# Challenge Two: You Got Mail

## How To Compile
1. Create a file called config.json next to `ModBot.csproj` the and input this json with the proper information.
```json
{
  "botToken": "Bot Token",
  "modmailChannelID": ChannelID
}```
2. CD into the directory of `ModBot.csproj` and type `dotnet run ModBot.csproj`. This will run it immediately. If you want to compile, type `dotnet build` and then cd into \bin\Debug\netcoreapp2.1 and type `dotnet ModBot.dll`

## Introduction:
It was brought up in The Council's most recent meeting ~~(FM mentioned it in a random message in our channel)~~ that a Mod Mail feature would be quite helpful for the server. In essence, it would be a way for you to communicate with The Council as a whole without other members seeing, to provide feedback or bring something to our attention. Now, we're extremely busy running the server (if you couldn't already tell), and that's where you come in: we'd like to give you the opportunity to create something the whole server will use! Thus, this week's challenge is to create a Mod Mail Bot containing according to the specifications below. Note that the winning submission may be modified at our discretion to bring it in line with our style, improve it, or any number of reasons. I'm also not pinging for this one, because it's too long for a single message and I don't want to ping twice.

## Objectives:
1. The primary objective of this challenge is to create a Mod Mail Bot containing the features set out below.
2. DMing the bot should result in the message being relayed to a specific channel in the server for The Council to see.
3. Council members should be able to reply to individual users via the bot also, with the option of replying anonymously or not.
4. The messages both ways need to be formatted neatly and efficiently, so it is easy to instantly see the message, but also presents other essential information.
5. There should be a banning system so that users who abuse the Mod Mail can be blacklisted from it, but still have access to the server.

## Submission: 
1. Create a fork of this repo.
2. Create a folder within the repo named with your name.
3. Work inside that folder.
4. Create a pull request to the original repo when done.
For help on the above steps, see <https://gist.github.com/Chaser324/ce0505fbed06b947d962>