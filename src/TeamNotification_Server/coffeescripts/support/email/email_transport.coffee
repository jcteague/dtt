Q = require('q')
_ = require('underscore')
logger = require('../logging/logger')

transport = transport_factory = require('./email_transport_factory').get()

send_email = (email_message) ->
    deferred = Q.defer()
    emails = sanitize_emails email_message
    if emails.to is ''
        logger.warn "Emails not sent. To address does not contain any valid recipient"
        deferred.reject 'Malformed email addresses'
    else
        transport.sendMail emails, (error, response) ->
            if error
                logger.error "Email could not be sent", {error: error}
                deferred.reject error
            else
                logger.info "Email sent", {response: response}
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
