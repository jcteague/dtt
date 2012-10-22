config = require('../../config')()
logentries = require('node-logentries')
winston = require('winston')

logentries_log = logentries.logger { token: config.log.token }

logentries_log.winston winston, level: 'info'
winston.add winston.transports.File, filename: config.log.path

module.exports = winston
