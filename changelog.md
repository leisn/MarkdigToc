### 0.1.3
------

* Fix parse two markdown document use same `MarkdownPipelineBuilder` , one document without `[toc]` and the other one has, the generated toc block will contains both. This bug is occur case I use the `TocOptions.Headings` to store the headings, and don't clear them when all renderer completed, not just clear after toc rendered.
* After fixed, markdown document can has more than one `[toc]` , and all of these can be rendered, though it's useless.

