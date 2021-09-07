# MarkdigToc  [![NuGet](https://img.shields.io/nuget/v/Leisn.MarkdigToc)](https://www.nuget.org/packages/Leisn.MarkdigToc/)

MarkdigToc is a extension for Markdig to generate table of content by parse [toc] in markdown content. 

> Currently just for render to html.

## Usage

Use with default options:


```csharp
var pipeline = new MarkdownPipelineBuilder()
    .UseAdvancedExtensions() // Add most of all advanced extensions
    .UseTableOfContent() //Add MarkdigToc extension
    .Build();
var result=Markdown.ToHtml(@"
[TOC]
# t1
## t1.1
### t1.1.1
### t1.1.2
## t1.2
");
Console.WriteLine(result);
```

Use with custom options:

```csharp
var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions() 
    .UseTableOfContent(
        tocAction: opt=>{ // toc options },
        idAction: opt=>{ // auto id options }
	).Build();

// ...
```

## Options

### CustomAutoIdOptions

Code copied from `AutoIdentifierExtension`, then added some code and options.

> NOTICE: When use `UseTableOfContent`, it will auto replace (`AutoIdentifierExtension`) or add (`CustomAutoIdExtension`).

* `AutoIdentifierOptions`  **Options**:  `Enum: default AutoIdentifierOptions.Default`

  Option from `AutoIdentifierExtension`: 

  * None
  * AutoLink
  *  AllowOnlyAscii
  *  Default
  * Github

* `GenerateHeadingId? ` **HeadingIdGenerator**: ` Delegate: defalut null`

  Delegate for handle custom heading id creation.

  Arguments:

  * level:  `int`

    The level of current heading, usually be count of char **#**.

  * content:  `string`

    The content of current heading.

  * id:  `string?`

    Not null if already defined id in markdown strings.

    e.g.   *title-id*  for  `# title {#title-id}`
    
    > NOTICE:  In order to parse attributes, need `UseGenericAttributes` extension after all of other extensions which you want parse.

### TocOptions

#### Specials

* ` IsUlOnlyContainLi`:  `bool: default true`

  >According to [webhint](https://webhint.io/docs/user-guide/hints/hint-axe/structure/?source=devtools) , `ul` and `ol` must only directly contain `li`, `script` or `template` elements.

  Set **false** to mix `ul` and `li` like others do (generate less code).
  
* `TitleAsConainerHeader`: `bool: default false`

  Put the tile in `ContainerTag` not inside the` TocTag`

  > NOTICE: working only `ContainerTag` is not null.

#### TOC Container

* `ContainerTag`: `string? : default null`

  If this is not null, the toc will put in a element use `ContainerTag`.

* `ContainerId`: `string? : default null`

  Id attribute for  `ContainerTag`.

* `ContainerClass`: `string? : default null`

  Class attribute for  `ContainerTag`.  e.g. `"class1 class2"`

#### TOC Element

* `TocTag`: `string : default nav`

  Tag name for toc element.

* `TocId`: `string? : default null`

  Id attribute for  `TocTag`.

* `TocClass`: `string? : default null`

  Class attribute for  `TocTag`.  e.g. `"class1 class2"`

#### TOC Title

> NOTICE: I also parse toc title and use it's attributes from markdown content , but that is not a regular *syntax*, you should know that.

* `OverrideTitle`: `string? : default null`

  Override toc title , ignore defined in markdown content.

* `TitleTag`: `string : default p`

  Tag name for toc title element.

* `TitleId`: `string? : default null`

  Id attribute for  `TitleTag`.

* `TitleClass`: `string? : default null`

  Class attribute for  `TitleTag`.  e.g. `"class1 class2"`

#### TOC Items

* `ulClass`: `string? : default p`

  Class attribute for  `ul`  element.

* `liClass`: `string? : default null`

  Class attribute for  `li`  element.

* `aClass`: `string? : default null`

  Class attribute for  `a`  element.



## Others

Markdown content:

```markdown
[TOC]       

##### t5
#### t4
### t3
## t2
# t1
## t2
### t3
#### t4
##### t5
```

`IsUlOnlyContainLi=true` :

```
●
    ○
        ■
            ■
                ■ t5
            ■ t4
        ■ t3
    ○ t2
● t1
    ○ t2
        ■ t3
            ■ t4
                ■ t5
```

`IsUlOnlyContainLi=false` :

```
                ■ t5
            ■ t4
        ■ t3
    ○ t2
● t1
    ○ t2
        ■ t3
            ■ t4
                ■ t5
```

