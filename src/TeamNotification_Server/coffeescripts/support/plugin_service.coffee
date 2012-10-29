fs = require('fs')
config = require('../config')()
file_reader = require('./system/file_reader')
functional = require('./functional')
Q = require('q')

create_read_stream_promise = functional.curry(Q.ncall, fs.createReadStream, fs)

get_filestream = ->
    file_reader.read(config.plugins.visual_studio.installer).then create_read_stream_promise

module.exports =
    get_filestream: get_filestream
