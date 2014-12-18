<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	
	<xsl:output method="text" indent="yes"/>
	<xsl:template match="*/text()[normalize-space()]">
		<xsl:value-of select="normalize-space()"/>
	</xsl:template>

	<xsl:template match="*/text()[not(normalize-space())]" />

	<xsl:template match="/">#include "mpi.h";
<xsl:for-each select="//Vertices/CGBlock[Level>0][not(preceding::Content = Content)]"> void <xsl:value-of select="Alias"/>(<xsl:variable name="CurrentId" select="Id"/><xsl:for-each select="//Edges/Link[To=$CurrentId]">var In_<xsl:value-of select="position()"/><xsl:if test="position()!=last()">,</xsl:if></xsl:for-each>)
{
	return In_1<xsl:value-of select="Content"/>In_2;
}
</xsl:for-each>
int main(int argc, char *argv[]){ 
{
    var result=0;
    MPI_Init(&amp;argc,&amp;argv);

    int rank;
    MPI_Comm_rank(MPI_COMM_WORLD, &amp;rank);

    int size;
    MPI_Comm_size(MPI_COMM_WORLD, &amp;size);
      
    <xsl:for-each select="//Vertices/CGBlock[Level=0][number(Content)!=Content]">var <xsl:value-of select="Content"/>;
    </xsl:for-each>
    
    <xsl:for-each select="//Vertices/CGBlock[Level=0][number(Content)=Content]">const<xsl:if test="contains(Content,'.')"> double </xsl:if><xsl:if test="not(contains(Content,'.'))"><xsl:if test="(Content='true') or (Content='false')"> bool </xsl:if><xsl:if test="(Content!='true') and (Content!='false')"> int </xsl:if></xsl:if> const_<xsl:value-of select="position()"/>=<xsl:value-of select="Content"/>;
    </xsl:for-each>
    MPI_Finalize();
    return result;
}
</xsl:template>
</xsl:stylesheet>
