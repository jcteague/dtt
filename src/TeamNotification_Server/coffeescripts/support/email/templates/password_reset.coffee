config = require('../../../config')()

get_template = (email, reset_key) ->
    reset_password_link = "#{config.site.url}/#/reset_password/#{reset_key}"
    return """
        <div>
            <p>Hello,</p>
            <p>You have recently asked for a password reset at <a href='#{config.site.url}'>YacketyApp.com</a></p>
            <p>
                To proceed with the password reset process, just click on the link below otherwise just ignore this email
            </p>
            <a href="#{reset_password_link}">#{reset_password_link}</a>
            <br />
            <p>The Yackety App Team</p>
        </div>
    """

using = (values) ->
    return {
        from: 'Password reset <invitations@yacketyapp.com>'
        to: values.email
        subject: 'You have requested a password reset at Yackety App'
        html: get_template(values.email, values.reset_key)
    }

module.exports =
    using: using
