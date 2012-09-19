nodemailer = require('nodemailer')
smtp_options = require('./gmail_smtp_options')

get = ->
    nodemailer.createTransport 'SMTP', smtp_options

module.exports =
    get: get
