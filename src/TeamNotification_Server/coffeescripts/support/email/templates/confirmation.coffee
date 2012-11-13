config = require('../../../config')()

get_template = (name, confirmation_key) ->
    confirmation_link = "#{config.site.url}/#/user/confirm/#{confirmation_key}"
    return """
        <div>
            <p>Hi #{name},</p>
            <p>Welcome to Yackety App, </p>
            <p>
                But before you start using our services you'll need to confirm your account, just click on the link below:
            </p>
            <a href="#{confirmation_link}">#{confirmation_link}</a>
            <br />
            <p>The Yackety App Team</p>
        </div>
    """

using = (fields) ->
    return {
        from: 'User Confirmations <invitations@yacketyapp.com>'
        to: fields.email
        subject: "Yackety App user confirmation"
        html: get_template(fields.name, fields.confirmation_key)
    }

module.exports =
    using: using
