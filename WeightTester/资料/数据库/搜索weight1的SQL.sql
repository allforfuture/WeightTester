SELECT dd.inspect_text
FROM t_insp_j11 AS ii
LEFT JOIN m_process AS pp ON pp.proc_uuid = ii.proc_uuid
LEFT JOIN t_data_j11 AS dd ON dd.insp_seq = ii.insp_seq
WHERE ii.serial_cd = 'GH9123456789XXXXX'
AND ii.datatype_id='WATERINJECT'
AND pp.site_cd='NSTD'
AND pp.factory_cd='M-2'
AND pp.line_cd = 'V02'
AND pp.process_cd = 'AE-4'
AND dd.inspect_cd ='BEFORE_WATER_INJECTION'
ORDER BY ii.process_at DESC LIMIT 1

    <!--pqm数据库查找weight1-->
    <add key="site_cd" value="NSTD" />
    <add key="factory_cd" value="M-2" />
    <add key="line_cd" value="V02" />
    <add key="process_cd" value="AE-4" />
    <add key="inspect_cd" value="BEFORE_WATER_INJECTION" />
    <!--<add key="datatype_id" value="MODULE" />-->
    <add key="datatype_id" value="WATERINJECT" />