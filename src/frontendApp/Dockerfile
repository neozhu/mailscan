FROM node:18-alpine AS build

WORKDIR /app
COPY . .
RUN yarn
RUN yarn build

FROM node:18-alpine AS deploy-node

WORKDIR /app
RUN rm -rf ./*
COPY --from=build /app/package.json .
COPY --from=build /app/build .
RUN yarn install --production && \
  yarn cache clean
ENV PUBLIC_POCKETBASE_URL=http://10.33.1.166:3010
ENV BODY_SIZE_LIMIT=651160
CMD ["node", "index.js"]