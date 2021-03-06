﻿/// <reference path="jquery-2.1.1.js" />
/// <autosync enabled="true" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="../amphiprioncms/scripts/application.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/build-config.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/ckeditor.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/config.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/styles.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/adapters/jquery.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/af.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ar.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/bg.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/bn.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/bs.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ca.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/cs.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/cy.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/da.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/de.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/el.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/en-au.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/en-ca.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/en-gb.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/en.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/eo.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/es.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/et.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/eu.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/fa.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/fi.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/fo.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/fr-ca.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/fr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/gl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/gu.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/he.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/hi.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/hr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/hu.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/id.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/is.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/it.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ja.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ka.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/km.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ko.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ku.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/lt.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/lv.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/mk.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/mn.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ms.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/nb.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/nl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/no.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/pl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/pt-br.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/pt.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ro.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ru.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/si.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/sk.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/sl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/sq.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/sr-latn.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/sr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/sv.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/th.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/tr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/ug.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/uk.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/vi.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/zh-cn.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/lang/zh.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/samples/sample.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/dialog/dialogdefinition.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/a11yhelp.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/about/dialogs/about.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/clipboard/dialogs/paste.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/image/dialogs/image.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/link/dialogs/anchor.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/link/dialogs/link.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/pastefromword/filter/default.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/scayt/dialogs/options.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/specialchar.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/table/dialogs/table.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/tabletools/dialogs/tablecell.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/wsc/dialogs/wsc.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/wsc/dialogs/wsc_ie.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/samples/assets/uilanguages/languages.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/ar.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/bg.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/ca.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/cs.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/cy.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/da.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/de.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/el.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/en.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/eo.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/es.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/et.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/fa.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/fi.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/fr-ca.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/fr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/gl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/gu.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/he.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/hi.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/hr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/hu.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/id.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/it.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/ja.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/km.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/ko.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/ku.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/lt.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/lv.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/mk.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/mn.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/nb.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/nl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/no.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/pl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/pt-br.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/pt.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/ro.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/ru.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/si.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/sk.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/sl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/sq.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/sr-latn.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/sr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/sv.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/th.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/tr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/ug.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/uk.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/vi.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/zh-cn.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/a11yhelp/dialogs/lang/zh.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/ar.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/bg.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/ca.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/cs.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/cy.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/de.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/el.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/en.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/eo.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/es.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/et.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/fa.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/fi.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/fr-ca.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/fr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/gl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/he.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/hr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/hu.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/id.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/it.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/ja.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/km.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/ku.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/lv.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/nb.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/nl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/no.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/pl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/pt-br.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/pt.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/ru.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/si.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/sk.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/sl.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/sq.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/sv.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/th.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/tr.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/ug.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/uk.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/vi.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/zh-cn.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/plugins/specialchar/dialogs/lang/zh.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/samples/plugins/dialog/assets/my_dialog.js" />
/// <reference path="../amphiprioncms/scripts/ckeditor/samples/plugins/htmlwriter/assets/outputforflash/swfobject.js" />
/// <reference path="../amphiprioncms/scripts/bootstrap.js" />
/// <reference path="../amphiprioncms/scripts/respond.matchmedia.addlistener.js" />
/// <reference path="../amphiprioncms/scripts/respond.js" />
