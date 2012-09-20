nodemailer = require('nodemailer')
smtp_options = require('./email_configuration')

get = ->
    # TODO: When having set up a correct non production email sending environment, remove this
    return require('../../test/helpers/mock_email_transport').get() if process.env.NODE_ENV is 'test'

    nodemailer.createTransport 'SMTP', smtp_options

module.exports =
    get: get
