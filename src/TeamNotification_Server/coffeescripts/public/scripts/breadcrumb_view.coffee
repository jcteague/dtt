define 'breadcrumb_view', ['backbone', 'config','general_view'], (Backbone, config, GeneralView) ->

    class BreadcrumbView extends GeneralView
        id: 'breadcrumb-container'

        ul: $("""<ul class="breadcrumb"></ul>""")
        
        initialize: ->


        render: ->
            @$el.empty()
            @ul.empty()
            @$el.append @ul
            @ul.append """<li class="active">Home</li>"""
            @
