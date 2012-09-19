config = require('../../config')()
options =
    service: 'SES'
    auth:
        user: config.email.user
        pass: config.email.password

module.exports = options
