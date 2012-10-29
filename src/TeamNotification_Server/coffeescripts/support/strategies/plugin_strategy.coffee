config = require('../../config')()
file_reader = require('../system/file_reader')

strategy = () ->
    file_reader.read_as_json(config.plugins.visual_studio.manifest)

module.exports = strategy
