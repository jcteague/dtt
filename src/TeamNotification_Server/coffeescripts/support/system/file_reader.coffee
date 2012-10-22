Q = require('q')
fs = require('fs')

read = (file_path) ->
    Q.ncall fs.readFile, fs, file_path, 'utf8'

read_as_json = (file_path) ->
    read(file_path).then (file) ->
        JSON.parse(file)

module.exports =
    read: read
    read_as_json: read_as_json
