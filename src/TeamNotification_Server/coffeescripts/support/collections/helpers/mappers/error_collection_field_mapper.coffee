map = (error) ->
    return {
        title: error.title
        message: error.message
    }

module.exports =
    map: map
