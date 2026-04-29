# Seq.App.Telegram.Advanced 🚀

An **advanced fork** of [stdray/Seq.App.Telegram](https://github.com/stdray/Seq.App.Telegram) for [Seq](https://datalust.co/) that enables seamless forwarding of log events to Telegram. This version updates the underlying **Telegram.Bot** library and introduces several new features:

- **Bot Server BaseUrl** — Customize the base URL to connect to alternative Bot API servers.
- **Forum Supergroups & Topics** — Target specific discussion threads within supergroups using `message_thread_id`.
- **Silent Messages** — Send notifications as silent messages with `disable_notification`, allowing users to receive alerts without sound.
- **Flattened Nested Properties** — Flatten nested event properties into dot-notation keys (e.g., `User.FirstName`) for accurate resolution and display within `MessageTemplate`.

[![NuGet](https://img.shields.io/nuget/v/Seq.App.Telegram.Advanced.svg?style=flat-square)](https://www.nuget.org/packages/Seq.App.Telegram.Advanced/)
[![Build status](https://ci.appveyor.com/api/projects/status/4pqo1oa5gm2e2kun/branch/master?svg=true)](https://ci.appveyor.com/project/mveism/seq-app-telegram-advanced/branch/master)


## Highlights & Updates ✨

- **Conditional Link Appending** — Now, if the **Seq Base URL** setting is empty, the message will no longer include a link at the end. This prevents broken or unnecessary links, ensuring cleaner message outputs.

- **Socks5 Proxy Support** — Proxy support remains intact for users requiring network routing through Socks5 proxies.

---

## Installation & Requirements 📋

### Prerequisites

- **Seq 2021.4+** — This plugin is compatible with and requires **Seq version 2021.4 or higher**.
- **Telegram Bot Token** — Create or use an existing bot. Refer to the [Telegram Bot API documentation](https://core.telegram.org/bots/api#authorizing-your-bot) for setup instructions.
- **Chat ID** — Obtain the chat ID by inviting [@ShowJsonBot](https://telegram.me/ShowJsonBot) or [@RawDataBot](https://telegram.me/RawDataBot) into your chat. They will reply with the chat details, including the `id`.

#### Example JSON:
```json
"chat": {
  "title": "Sample Group",
  "type": "group",
  "all_members_are_administrators": true,
  "id": -221908654
}
```

---

## Sending Messages to Specific Topics & Supergroups 🗂️

If you wish to target a particular discussion thread within a supergroup (forum), invite [@ShowJsonBot](https://telegram.me/ShowJsonBot) into the group. The bot will send back a JSON payload containing the `message_thread_id` needed for configuration.

### Example JSON:
```json
{
  "chat": {
    "id": -1009999999999,
    "title": "Discussion Group",
    "is_forum": true,
    "type": "supergroup"
  },
  "message_thread_id": 3,   // Use this ID to send messages to the specific topic
  "forum_topic_created": {
    "name": "SeqLogsTopic",
    "icon_color": 16478047
  },
  "is_topic_message": true
}
```

---