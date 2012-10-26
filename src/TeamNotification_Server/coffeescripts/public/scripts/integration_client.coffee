#request = new XMLHttpRequest()
#request.open 'GET', 'https://api.dtt.local:3001/registration'

$ ->
    callback = (r) ->
        $('body').append('<h1/>').text('Hello')
        console.log l for l in r.links

    $.support.cors = true

    parameters = {
        type: 'GET'
        dataType: 'json'
        url: 'https://api.dtt.local:3001/'
        success: callback
        error: (d) -> console.log('error is', d)
    }

    $.ajax parameters
    alert 'done new'
