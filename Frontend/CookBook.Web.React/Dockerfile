FROM node:14.17-alpine

WORKDIR /app

ENV PATH /app/node_modules/.bin:$PATH

COPY package.json ./
# COPY package-lock.json ./
RUN yarn install --silent
# RUN npm install react-scripts@3.4.1 -g

COPY . ./

CMD ["yarn", "start"]