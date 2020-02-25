// ts2fable 0.6.2
module rec Cheerio

open System
open Fable.Core
open Fable.Core.JS

[<ImportAll("cheerio")>]
let cheerio: CheerioAPI = jsNative

[<AllowNullLiteral>]
type AttrFunction =
    [<Emit "$0($1...)">]
    abstract Invoke: el:CheerioElement * i:float * currentValue:string -> obj option

[<AllowNullLiteral>]
type Cheerio =

    [<Emit "$0[$1]{{=$2}}">]
    abstract Item: index:float -> CheerioElement with get, set

    abstract cheerio: string with get, set
    abstract length: float with get, set
    abstract attr: unit -> CheerioAttrReturn
    abstract attr: name:string -> string
    abstract attr: name:string * value:AttrFunction -> Cheerio
    abstract attr: name:string * value:string -> Cheerio
    abstract attr: map:CheerioAttrMap -> Cheerio
    abstract data: unit -> obj option
    abstract data: name:string -> obj option
    abstract data: name:string * value:obj option -> obj option
    abstract ``val``: unit -> string
    abstract ``val``: value:string -> Cheerio
    abstract removeAttr: name:string -> Cheerio
    abstract has: selector:string -> Cheerio
    abstract has: element:CheerioElement -> Cheerio
    abstract hasClass: className:string -> bool
    abstract addClass: classNames:string -> Cheerio
    abstract removeClass: unit -> Cheerio
    abstract removeClass: className:string -> Cheerio
    abstract removeClass: func:(float -> string -> string) -> Cheerio
    abstract toggleClass: className:string -> Cheerio
    abstract toggleClass: className:string * toggleSwitch:bool -> Cheerio
    abstract toggleClass: ?toggleSwitch:bool -> Cheerio
    abstract toggleClass: func:(float -> string -> bool -> string) * ?toggleSwitch:bool -> Cheerio
    abstract is: selector:string -> bool
    abstract is: element:CheerioElement -> bool
    abstract is: element:ResizeArray<CheerioElement> -> bool
    abstract is: selection:Cheerio -> bool
    abstract is: func:(float -> CheerioElement -> bool) -> bool
    abstract serialize: unit -> string
    abstract serializeArray: unit -> ResizeArray<TypeLiteral_01>
    abstract find: selector:string -> Cheerio
    abstract find: element:Cheerio -> Cheerio
    abstract parent: ?selector:string -> Cheerio
    abstract parents: ?selector:string -> Cheerio
    abstract parentsUntil: ?selector:string * ?filter:string -> Cheerio
    abstract parentsUntil: element:CheerioElement * ?filter:string -> Cheerio
    abstract parentsUntil: element:Cheerio * ?filter:string -> Cheerio
    abstract prop: name:string -> obj option
    abstract prop: name:string * value:obj option -> Cheerio
    abstract closest: unit -> Cheerio
    abstract closest: selector:string -> Cheerio
    abstract next: ?selector:string -> Cheerio
    abstract nextAll: unit -> Cheerio
    abstract nextAll: selector:string -> Cheerio
    abstract nextUntil: ?selector:string * ?filter:string -> Cheerio
    abstract nextUntil: element:CheerioElement * ?filter:string -> Cheerio
    abstract nextUntil: element:Cheerio * ?filter:string -> Cheerio
    abstract prev: ?selector:string -> Cheerio
    abstract prevAll: unit -> Cheerio
    abstract prevAll: selector:string -> Cheerio
    abstract prevUntil: ?selector:string * ?filter:string -> Cheerio
    abstract prevUntil: element:CheerioElement * ?filter:string -> Cheerio
    abstract prevUntil: element:Cheerio * ?filter:string -> Cheerio
    abstract slice: start:float * ?``end``:float -> Cheerio
    abstract siblings: ?selector:string -> Cheerio
    abstract children: ?selector:string -> Cheerio
    abstract contents: unit -> Cheerio
    abstract each: func:(float -> CheerioElement -> obj option) -> Cheerio
    abstract map: func:(float -> CheerioElement -> obj option) -> Cheerio
    abstract filter: selector:string -> Cheerio
    abstract filter: selection:Cheerio -> Cheerio
    abstract filter: element:CheerioElement -> Cheerio
    abstract filter: elements:ResizeArray<CheerioElement> -> Cheerio
    abstract filter: func:(float -> CheerioElement -> bool) -> Cheerio
    abstract not: selector:string -> Cheerio
    abstract not: selection:Cheerio -> Cheerio
    abstract not: element:CheerioElement -> Cheerio
    abstract not: func:(float -> CheerioElement -> bool) -> Cheerio
    abstract first: unit -> Cheerio
    abstract last: unit -> Cheerio
    abstract eq: index:float -> Cheerio
    abstract get: unit -> ResizeArray<obj option>
    abstract get: index:float -> obj option
    abstract index: unit -> float
    abstract index: selector:string -> float
    abstract index: selection:Cheerio -> float
    abstract ``end``: unit -> Cheerio
    abstract add: selectorOrHtml:string -> Cheerio
    abstract add: selector:string * context:Document -> Cheerio
    abstract add: element:CheerioElement -> Cheerio
    abstract add: elements:ResizeArray<CheerioElement> -> Cheerio
    abstract add: selection:Cheerio -> Cheerio
    abstract addBack: unit -> Cheerio
    abstract addBack: filter:string -> Cheerio
    abstract appendTo: target:Cheerio -> Cheerio
    abstract prependTo: target:Cheerio -> Cheerio
    abstract append: content:string * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract append: content:Document * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract append: content:ResizeArray<Document> * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract append: content:Cheerio * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract prepend: content:string * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract prepend: content:Document * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract prepend: content:ResizeArray<Document> * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract prepend: content:Cheerio * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract after: content:string * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract after: content:Document * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract after: content:ResizeArray<Document> * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract after: content:Cheerio * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract insertAfter: content:string -> Cheerio
    abstract insertAfter: content:Document -> Cheerio
    abstract insertAfter: content:Cheerio -> Cheerio
    abstract before: content:string * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract before: content:Document * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract before: content:ResizeArray<Document> * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract before: content:Cheerio * [<ParamArray>] contents:ResizeArray<obj option> -> Cheerio
    abstract insertBefore: content:string -> Cheerio
    abstract insertBefore: content:Document -> Cheerio
    abstract insertBefore: content:Cheerio -> Cheerio
    abstract remove: ?selector:string -> Cheerio
    abstract replaceWith: content:string -> Cheerio
    abstract replaceWith: content:CheerioElement -> Cheerio
    abstract replaceWith: content:ResizeArray<CheerioElement> -> Cheerio
    abstract replaceWith: content:Cheerio -> Cheerio
    abstract replaceWith: content:(unit -> Cheerio) -> Cheerio
    abstract empty: unit -> Cheerio
    abstract html: unit -> string option
    abstract html: html:string -> Cheerio
    abstract text: unit -> string
    abstract text: text:string -> Cheerio
    abstract wrap: content:string -> Cheerio
    abstract wrap: content:Document -> Cheerio
    abstract wrap: content:Cheerio -> Cheerio
    abstract css: propertyName:string -> string
    abstract css: propertyNames:ResizeArray<string> -> ResizeArray<string>
    abstract css: propertyName:string * value:string -> Cheerio
    abstract css: propertyName:string * value:float -> Cheerio
    abstract css: propertyName:string * func:(float -> string -> string) -> Cheerio
    abstract css: propertyName:string * func:(float -> string -> float) -> Cheerio
    abstract css: properties:Object -> Cheerio
    abstract clone: unit -> Cheerio
    abstract toArray: unit -> ResizeArray<CheerioElement>

[<AllowNullLiteral>]
type CheerioAttrReturn =
    [<Emit "$0[$1]{{=$2}}">]
    abstract Item: attr:string -> string with get, set

[<AllowNullLiteral>]
type CheerioAttrMap =
    [<Emit "$0[$1]{{=$2}}">]
    abstract Item: key:string -> obj option with get, set

[<AllowNullLiteral>]
type CheerioOptionsInterface =
    abstract xmlMode: bool option with get, set
    abstract decodeEntities: bool option with get, set
    abstract lowerCaseTags: bool option with get, set
    abstract lowerCaseAttributeNames: bool option with get, set
    abstract recognizeCDATA: bool option with get, set
    abstract recognizeSelfClosing: bool option with get, set
    abstract normalizeWhitespace: bool option with get, set
    abstract ignoreWhitespace: bool option with get, set

[<AllowNullLiteral>]
type CheerioSelector =

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string * context:string -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string * context:CheerioElement -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string * context:ResizeArray<CheerioElement> -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string * context:Cheerio -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string * context:string * root:string -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string * context:CheerioElement * root:string -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string * context:ResizeArray<CheerioElement> * root:string -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:string * context:Cheerio * root:string -> Cheerio

    [<Emit "$0($1...)">]
    abstract Invoke: selector:obj option -> Cheerio

[<AllowNullLiteral>]
type CheerioStatic =
    inherit CheerioSelector
    abstract xml: unit -> string
    abstract root: unit -> Cheerio
    abstract contains: container:CheerioElement * contained:CheerioElement -> bool
    abstract parseHTML: data:string * ?context:Document * ?keepScripts:bool -> ResizeArray<Document>
    abstract html: ?options:CheerioOptionsInterface -> string
    abstract html: selector:string * ?options:CheerioOptionsInterface -> string
    abstract html: element:Cheerio * ?options:CheerioOptionsInterface -> string
    abstract html: element:CheerioElement * ?options:CheerioOptionsInterface -> string

[<AllowNullLiteral>]
type CheerioElement =
    abstract tagName: string with get, set
    abstract ``type``: string with get, set
    abstract name: string with get, set
    abstract attribs: TypeLiteral_02 with get, set
    abstract children: ResizeArray<CheerioElement> with get, set
    abstract childNodes: ResizeArray<CheerioElement> with get, set
    abstract lastChild: CheerioElement with get, set
    abstract firstChild: CheerioElement with get, set
    abstract next: CheerioElement with get, set
    abstract nextSibling: CheerioElement with get, set
    abstract prev: CheerioElement with get, set
    abstract previousSibling: CheerioElement with get, set
    abstract parent: CheerioElement with get, set
    abstract parentNode: CheerioElement with get, set
    abstract nodeValue: string with get, set
    abstract data: string option with get, set
    abstract startIndex: float option with get, set

[<AllowNullLiteral>]
type CheerioAPI =
    inherit CheerioSelector
    inherit CheerioStatic
    abstract load: html:string * ?options:CheerioOptionsInterface -> CheerioStatic
    abstract load: element:CheerioElement * ?options:CheerioOptionsInterface -> CheerioStatic

[<AllowNullLiteral>]
type Document =
    interface
    end

[<AllowNullLiteral>]
type TypeLiteral_01 =
    abstract name: string with get, set
    abstract value: string with get, set

[<AllowNullLiteral>]
type TypeLiteral_02 =
    [<Emit "$0[$1]{{=$2}}">]
    abstract Item: attr:string -> string with get, set
