const { createProxyMiddleware } = require("http-proxy-middleware");
const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
        ? env.ASPNETCORE_URLS.split(";")[0]
        : "http://localhost:39907";

const context = [
    "/api/countrylist",
    "/api/suppliergroup",
    "/api/suppliernews",
    "/api/languagelist",
    "/api/finance",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        proxyTimeout: 1000000,
        target: target,
        secure: false,
        headers: {
            Connection: "Keep-Alive",
        },
    });

    app.use(appProxy);
};
