FROM node:8.9.1-alpine

ENV NPM_CONFIG_LOGLEVEL warn

WORKDIR /code/

COPY package.json .
COPY yarn.lock .

RUN yarn install

COPY . .

EXPOSE 3000

CMD ["yarn", "start"]

