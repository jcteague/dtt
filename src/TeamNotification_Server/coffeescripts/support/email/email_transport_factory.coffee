nodemailer = require('nodemailer')
smtp_options = require('./email_configuration')

get = ->
    nodemailer.createTransport 'SMTP', smtp_options

module.exports =
    get: get
