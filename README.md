# 1337x telegram bot

Telegram bot, which easyly allowed to get magnetic links from http://1337x.to

Try it @ https://t.me/search_content_bot

## Getting Started
Create telegram bot: https://core.telegram.org/bots#3-how-do-i-create-a-bot

Open docker-compose.yaml change <telegram-token> to telegram bot token

### Prerequisites

What things you need to install the software and how to install them

```
Registred telegram bot
Docker or node 8/10 + mongo db
```


## Running the tests

Tests are written using ava-ts library.

Run test
```
yarn run test
```

## Deployment
Designed to be deploy on docker swarm cluster

Production dockerfile is Dockerfile.Release
## Built With

* [node-telegram-bot-api](https://github.com/yagop/node-telegram-bot-api) - telegram bot lib
* [cheerio](https://github.com/cheeriojs/cheerio) - html parser
* [request](https://github.com/request/request) - Simplified HTTP client

## Authors

* **Max "xbim" Gerasimov** - *Initial work* - [xbim](https://github.com/xbim)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
