package crc64a2cf4bd4cbdcc548;


public class ObservableCollectionAdapter_1
	extends crc64a2cf4bd4cbdcc548.ListAdapter_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Reactive.Bindings.ObservableCollectionAdapter`1, ReactiveProperty.Android", ObservableCollectionAdapter_1.class, __md_methods);
	}


	public ObservableCollectionAdapter_1 ()
	{
		super ();
		if (getClass () == ObservableCollectionAdapter_1.class)
			mono.android.TypeManager.Activate ("Reactive.Bindings.ObservableCollectionAdapter`1, ReactiveProperty.Android", "", this, new java.lang.Object[] {  });
	}

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
