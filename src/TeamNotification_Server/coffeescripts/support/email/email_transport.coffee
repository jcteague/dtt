Q = require('q')
_ = require('underscore')

transport = transport_factory = require('./email_transport_factory').get()

send_email = (email_message) ->
    deferred = Q.defer()
    emails = sanitize_emails email_message
    if emails.to is ''
        deferred.reject 'Malformed email addresses'
    else
        transport.sendMail emails, (error, response) ->
            if error
                #console.log 'email had error', error
                deferred.reject error
            else
                #console.log 'email was a success', response
                deferred.resolve response

    deferred.promise

sanitize_emails = (email_message) ->
    clone_message = _.clone email_message
    clone_message.to = (email_address.trim() for email_address in email_message.to.split(',') when is_valid_email(email_address)).join(',')
    clone_message

is_valid_email = (email) ->
    email.trim().match(/^(?:[\w\!\#\$\%\&\'\*\+\-\/\=\?\^\`\{\|\}\~]+\.)*[\w\!\#\$\%\&\'\*\+\-\/\=\?\^\`\{\|\}\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\-](?!\.)){0,61}[a-zA-Z0-9]?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\[(?:(?:[01]?\d{1,2}|2[0-4]\d|25[0-5])\.){3}(?:[01]?\d{1,2}|2[0-4]\d|25[0-5])\]))$/)


module.exports =
    send_email: send_email
