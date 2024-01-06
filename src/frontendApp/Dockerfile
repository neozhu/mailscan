FROM node:20-alpine AS build

WORKDIR /app
COPY . .
RUN yarn
RUN yarn build

FROM nginx:1.25.3-alpine-slim AS deploy-static

WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
COPY --from=build /app/build-static .
ENTRYPOINT ["nginx", "-g", "daemon off;"]

FROM node:20-alpine AS deploy-node

WORKDIR /app
RUN rm -rf ./*
COPY --from=build /app/package.json .
COPY --from=build /app/build-node .
CMD ["node", "index.js"]