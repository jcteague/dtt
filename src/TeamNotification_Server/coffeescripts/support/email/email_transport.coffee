Q = require('q')
nodemailer = require('nodemailer')
smtp_options = require('./gmail_smtp_options')

transport = nodemailer.createTransport 'SMTP', smtp_options

send_email = (email_message) ->
    deferred = Q.defer()
    transport.sendMail email_message, (error, response) ->
        if error
            deferred.reject error
        else
            deferred.resolve response

    deferred.promise

module.exports =
    send_email: send_email
