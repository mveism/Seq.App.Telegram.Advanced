# Seq.App.Telegram.Advanced

An advanced fork of [stdray/Seq.App.Telegram](https://github.com/stdray/Seq.App.Telegram) for [Seq](https://datalust.co/) that forwards log events to Telegram.  
This fork updates the Telegram.Bot library and adds support for:

- **Forum supergroups topics** — target a specific thread using `message_thread_id`
- **Silent messages** — send messages with `disable_notification` (users are notified with no sound)
- **Flatten nested properties** — nested event properties can now be flattened into dot-notation keys (e.g. `User.FirstName`) so they are correctly resolved and rendered in the `MessageTemplate`.

Socks5 proxy is still supported.

[![NuGet](https://img.shields.io/nuget/v/Seq.App.Telegram.Advanced.svg?style=flat-square)](https://www.nuget.org/packages/Seq.App.Telegram.Advanced/)
[![Build status](https://ci.appveyor.com/api/projects/status/4pqo1oa5gm2e2kun/branch/master?svg=true)](https://ci.appveyor.com/project/mveism/seq-app-telegram-advanced/branch/master)

---

### Requirements
* **Seq 2021.4+**  
  Older versions are not supported.
* **Bot authentication token**  
  You can use an existing bot token or create a new one. Refer to docs: https://core.telegram.org/bots/api#authorizing-your-bot
* **Chat id**  
  Invite [@ShowJsonBot](https://telegram.me/ShowJsonBot) or [@RawDataBot](https://telegram.me/RawDataBot) into your chat. They will send you a message with chat details. Copy the `id` value from the `chat` section, including the leading minus.

Example `chat` section:
```json
 "chat": {
   "title": "Some chat",
   "type": "group",
   "all_members_are_administrators": true,
   "id": -221908654
  },
  ```

Forum supergroups & topics
If you want to send a message into a specific topic inside a supergroup (forum), invite [@ShowJsonBot](https://telegram.me/ShowJsonBot) into the group.
The bot will send you JSON including the message_thread_id you can use in configuration:

Example `json`:
```json
{
  "chat": {
    "id": -1009999999999,
    "title": "Some chat",
    "is_forum": true,
    "type": "supergroup"
  },
  "message_thread_id": 3,   // Use this ID to target the topic
  "forum_topic_created": {
    "name": "SeqLogsTopic",
    "icon_color": 16478047
  },
  "is_topic_message": true
}
```