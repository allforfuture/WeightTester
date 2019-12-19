    <add key="site_cd" value="NSTD" />
    <add key="factory_cd" value="M-2" />
    <add key="line_cd" value="L01" />
    <add key="process_cd" value="AE-4" />
    <add key="inspect_cd" value="BEFORE_WATER_INDECTION" />
    <add key="datatype_id" value="MODULE" />
SELECT dd.inspect_text
FROM t_insp_j11 AS ii
LEFT JOIN m_process AS pp ON pp.proc_uuid = ii.proc_uuid
LEFT JOIN t_data_j11 AS dd ON dd.insp_seq = ii.insp_seq
WHERE ii.serial_cd = 'HRD9087-001D9GK09-DCCI1600A'
AND ii.datatype_id='MODULE'
AND pp.site_cd='NSTD'
AND pp.factory_cd='M-2'
AND pp.line_cd = 'L01'
AND pp.process_cd = 'AE-4'
AND dd.inspect_cd ='BEFORE_WATER_INDECTION'
ORDER BY ii.process_at DESC LIMIT 1

    <add key="site_cd" value="NSTD" />
    <add key="factory_cd" value="M-2" />
    <add key="line_cd" value="L01" />
    <add key="process_cd" value="AE-6" />
    <add key="inspect_cd" value="WEIGHT_AFTER_VACUUM" />
    <add key="datatype_id" value="WATERINJECT" />
SELECT dd.inspect_text
FROM t_insp_j11 AS ii
LEFT JOIN m_process AS pp ON pp.proc_uuid = ii.proc_uuid
LEFT JOIN t_data_j11 AS dd ON dd.insp_seq = ii.insp_seq
WHERE ii.serial_cd = 'HRD9087-001D9GK09-DCCI1600A'
AND ii.datatype_id='WATERINJECT'
AND pp.site_cd='NSTD'
AND pp.factory_cd='M-2'
AND pp.line_cd = 'L01'
AND pp.process_cd = 'AE-6'
AND dd.inspect_cd ='WEIGHT_AFTER_VACUUM'
ORDER BY ii.process_at DESC LIMIT 1