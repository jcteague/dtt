fs = require('fs')

exports = {}
exports.require_all_files_in  = (path,app,io) ->
    fs.readdirSync(path).forEach (file) ->
        stats = fs.lstatSync(path+ '/' +file)
        return if !stats.isDirectory() && (file.substr(-3) != '.js' || file == "index.js")

        name = file
        if stats.isFile()
            name = name.substr(0, name.indexOf('.'))

        full_path = path + '/' + name
        require(full_path).build_routes(app,io)

module.exports = exports
