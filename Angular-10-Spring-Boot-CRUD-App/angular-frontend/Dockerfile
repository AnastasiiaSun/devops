FROM node:14

WORKDIR /app

COPY . .

RUN npm cache verify
RUN npm install

EXPOSE 4200

CMD ["npm", "start"]