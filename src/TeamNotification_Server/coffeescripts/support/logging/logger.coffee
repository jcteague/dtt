config = require('../../config')()
logger = require('winston')
logger.add logger.transports.File, filename: config.log.path

module.exports = logger
