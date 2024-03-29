define 'breadcrumb_view', ['backbone', 'config','general_view'], (Backbone, config, GeneralView) ->

    class BreadcrumbView extends GeneralView
        id: 'breadcrumb-container'

        initialize: ->


        render: ->
            @$el.empty()
            @$el.innerHTML = ""
            ul = $("""<ul class="breadcrumb"></ul>""")
            @$el.append ul
            if @model.has('breadcrumb')
                breadcrumb_links = @model.get('breadcrumb')
                current=null
                for breadcrumb in breadcrumb_links  
                    if breadcrumb.rel != 'active'
                        ul.append """<li><a href="##{breadcrumb.href}">#{breadcrumb.name}</a> <span class="divider">/</span> </li>"""
                    else
                        current = breadcrumb
                if current?
                    ul.append """<li class="active"> #{current.name}</li> """
            else
                ul.append """<li class="active">Home</li>"""
            @
