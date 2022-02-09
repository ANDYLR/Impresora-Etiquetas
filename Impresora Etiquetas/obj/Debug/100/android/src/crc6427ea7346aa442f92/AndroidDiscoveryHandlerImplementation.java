package crc6427ea7346aa442f92;


public class AndroidDiscoveryHandlerImplementation
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.zebra.sdk.printer.discovery.DiscoveryHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_discoveryError:(Ljava/lang/String;)V:GetDiscoveryError_Ljava_lang_String_Handler:Zebra.Sdk.Printer.Discovery.IDiscoveryHandlerInvoker, ZSDK_ANDROIDX\n" +
			"n_discoveryFinished:()V:GetDiscoveryFinishedHandler:Zebra.Sdk.Printer.Discovery.IDiscoveryHandlerInvoker, ZSDK_ANDROIDX\n" +
			"n_foundPrinter:(Lcom/zebra/sdk/printer/discovery/DiscoveredPrinter;)V:GetFoundPrinter_Lcom_zebra_sdk_printer_discovery_DiscoveredPrinter_Handler:Zebra.Sdk.Printer.Discovery.IDiscoveryHandlerInvoker, ZSDK_ANDROIDX\n" +
			"";
		mono.android.Runtime.register ("LinkOS.Plugin.AndroidDiscoveryHandlerImplementation, LinkOS.Plugin", AndroidDiscoveryHandlerImplementation.class, __md_methods);
	}


	public AndroidDiscoveryHandlerImplementation ()
	{
		super ();
		if (getClass () == AndroidDiscoveryHandlerImplementation.class)
			mono.android.TypeManager.Activate ("LinkOS.Plugin.AndroidDiscoveryHandlerImplementation, LinkOS.Plugin", "", this, new java.lang.Object[] {  });
	}


	public void discoveryError (java.lang.String p0)
	{
		n_discoveryError (p0);
	}

	private native void n_discoveryError (java.lang.String p0);


	public void discoveryFinished ()
	{
		n_discoveryFinished ();
	}

	private native void n_discoveryFinished ();


	public void foundPrinter (com.zebra.sdk.printer.discovery.DiscoveredPrinter p0)
	{
		n_foundPrinter (p0);
	}

	private native void n_foundPrinter (com.zebra.sdk.printer.discovery.DiscoveredPrinter p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
