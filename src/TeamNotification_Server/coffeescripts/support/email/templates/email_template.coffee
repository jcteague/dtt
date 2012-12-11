for_object =
    invitation: require('./invitation')
    confirmation: require('./confirmation')
    password_reset: require('./password_reset')
    added_to_room: require('./added_to_room')
module.exports =
    for: for_object
