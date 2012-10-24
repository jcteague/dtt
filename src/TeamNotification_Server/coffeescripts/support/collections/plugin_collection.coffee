Q = require('q')

class PluginCollection

    constructor: (@data) ->


    to_json: ->
        plugin =
            data: [
                {"name": "name", "value": @data.name}
                {"name": "version", "value": @data.version}
            ],
            links: [
                #{"rel": "Plugin", "name": @data.name, "href": "/plugin/download/#{@data.file_name}"}
                {"rel": "Plugin", "name": @data.name, "href": "https://s3.amazonaws.com/yackety-vs-plugin/TeamNotification_Package.vsix"}
            ]

        return {
            href: '/plugin'
            plugin: plugin
        }

    fetch_to: (callback) ->
        Q.resolve(@to_json()).then callback

module.exports = PluginCollection
