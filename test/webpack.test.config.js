var path = require("path");
var webpack = require("webpack");

module.exports = {
    mode: "development",
    target: "node",
    entry: path.join(__dirname, "./Torrent1337Bot.Tests/Torrent1337Bot.Tests.fsproj"),
    output: {
        path: path.join(__dirname, "./bin"),
        filename: "bundle.js",
    },
    module: {
        rules: [{
            test: /\.fs(x|proj)?$/,
            use: "fable-loader"
        }]
    },
    plugins: [
        new webpack.NamedModulesPlugin()
    ]
}