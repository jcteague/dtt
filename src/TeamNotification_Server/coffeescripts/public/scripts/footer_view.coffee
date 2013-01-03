define 'footer_view', ['general_view','jquery'], (GeneralView, $) ->
    class FooterView  extends GeneralView
        id: 'footer-container'
        
        initialize: ->
        
        render: ->
            @$el.empty()
            @$el.attr('class', 'footer')
            @$el.append """<div class='container'>
                <p class='muted credit'> Thanks <a href='http://twitter.github.com/bootstrap/'>bootstrap</a> - More credits here too </p>
            </div>"""
            @
