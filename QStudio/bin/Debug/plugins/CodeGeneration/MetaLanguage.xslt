<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/">
    <Program>
      <InitIncoming>
        <xsl:for-each select="//Vertices/CGBlock[Level=0]">
          <xsl:if test="number(Content)!=Content">
            <Variable>
              <xsl:attribute name="name">variable_<xsl:value-of select="Id" /></xsl:attribute>
              <xsl:attribute name="type"><xsl:value-of select="CGType" /></xsl:attribute>
            </Variable>
          </xsl:if>
          <xsl:if test="number(Content)=Content">
            <Constant>
              <xsl:attribute name="name">variable_<xsl:value-of select="Id" /></xsl:attribute>
              <xsl:attribute name="type"><xsl:value-of select="CGType" /></xsl:attribute>
            </Constant>
          </xsl:if>
        </xsl:for-each>
    </InitIncoming>
    <xsl:for-each select="//Vertices/CGBlock[Level>0][not(preceding::Level = Level)]">
      <Level>
        <xsl:variable name="NumberOfLevel" select="Level"/>
        <xsl:attribute name="number"><xsl:value-of select="$NumberOfLevel" /></xsl:attribute>
        <xsl:for-each select="//Vertices/CGBlock[Level=$NumberOfLevel]">
          <CPU>
            <xsl:variable name="NumberOfCPU" select="position()"/>
            <xsl:variable name="CurrentId" select="Id"/>
            <xsl:attribute name="number"><xsl:value-of select="$NumberOfCPU" /></xsl:attribute>
            <Function>
              <xsl:attribute name="title"><xsl:value-of select="Alias" /></xsl:attribute>
              <InVariables>
                <xsl:for-each select="//Edges/Link[To=$CurrentId]">
                  <Variable>
                    <xsl:attribute name="name">variable_<xsl:value-of select="From" /></xsl:attribute>
                  </Variable>
                </xsl:for-each>
              </InVariables>
              <OutVariable>
                <xsl:attribute name="name">variable_<xsl:value-of select="$CurrentId" /></xsl:attribute>
              </OutVariable>
            </Function>
          </CPU>
        </xsl:for-each>    
      </Level>
    </xsl:for-each>
  </Program>
  </xsl:template>
</xsl:stylesheet>
